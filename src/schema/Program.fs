open System.Text

open Microsoft.SqlServer.Management.Smo
open Microsoft.SqlServer.Management.Common

open Nessos.UnionArgParser

let mk_server (server : string) (user : string) (password : string) =
  Server(ServerConnection(server, user, password))

let export file_path (sqlServer : Server) (database : string) =
  let opts =
    ScriptingOptions(
      ExtendedProperties = true,
      Indexes = true,
      Triggers = true,
      ScriptBatchTerminator = true,
      Encoding = Encoding.UTF8,
      FileName = file_path,
      IncludeHeaders = true,
      ToFileOnly = true,
      DriAll = true)

  printfn "finding db '%s'" database
  let db = sqlServer.Databases.[database]
  printfn "starting transfer"
  let tf = Transfer(db, Options = opts)
  printfn "%O" (tf.ScriptTransfer())

type CLIArguments =
  | Server of string
  | User of string
  | Password of string
  | Database of string
  | FilePath of string
  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Server _   -> "server host name/dns name"
      | User _     -> "the username for the database"
      | Password _ -> "the password for the database"
      | Database _ -> "the name of the database (not the server's name)"
      | FilePath _ -> "the location to write the export to"

[<EntryPoint>]
let main argv =
  let parser  = UnionArgParser.Create<CLIArguments>()
  let results = parser.Parse argv
  let file    = results.GetResult <@ FilePath @>
  let server  = mk_server (results.GetResult <@ Server @>)
                          (results.GetResult <@ User @>)
                          (results.GetResult <@ Password @>)
  let db      = results.GetResult <@ Database @>
  export file server db
  0