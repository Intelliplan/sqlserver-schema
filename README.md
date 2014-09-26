# SQL Server - Schema

A tool that takes

 - A server DNS name
 - A Username for a DB
 - A Password for a DB
 - A FilePath for export target location

And spits out all schemas, triggers, etc to the target location file.

## Usage

```
schema --server "my.server.tld" --password SECRET --username Ulf --file-path './my-db.sql'
```

## Build

```
bundle
bundle exec rake
```