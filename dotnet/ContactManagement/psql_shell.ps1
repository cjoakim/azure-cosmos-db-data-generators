# PowerShell script to open a psql shell to one of several
# environments (localhost, flexible server, cosmos db)
# and optionally execute given SQL file.
#
# Chris Joakim, Microsoft, 2023

param(
    [Parameter()]
    [String]$env_name  = "<env>",
    [String]$db_name   = "<db>",
    [String]$psql_file = ""
)

$h=""
$p="5432"
$user="<user>"
$pass="<pass>"
$ssl=""

if ('local' -eq $env_name) {
    $h="localhost"
    $user=$env:LOCAL_PG_USER
    $pass=$env:LOCAL_PG_PASS
}
elseif ('flex' -eq $env_name)
{
    $h=$env:AZURE_PG_SERVER_FULL_NAME
    $user=$env:AZURE_PG_USER
    $pass=$env:AZURE_PG_PASS
    $ssl="sslmode=require"
}
elseif ('cosmos' -eq $env_name)
{
    if ('citus' -ne $db_name) {
        Write-Output "WARNING: 'citus' is the only allowed database name for Azure Cosmos DB / PostgreSQL"
    }
    $h=$env:AZURE_COSMOSDB_PG_SERVER_FULL_NAME
    $user=$env:AZURE_COSMOSDB_PG_USER
    $pass=$env:AZURE_COSMOSDB_PG_PASS
    $ssl="sslmode=require"
}
else {
    Write-Output ""
    Write-Output "Usage:"
    Write-Output ".\psql_shell.ps1 <env> <db>   where env is local, flex, or cosmos"
    Write-Output ".\psql_shell.ps1 <env> <db> <infile>"
    Write-Output ".\psql_shell.ps1 local dev"
    Write-Output ".\psql_shell.ps1 flex dev"
    Write-Output ".\psql_shell.ps1 cosmos citus"
    Write-Output ".\psql_shell.ps1 local dev psql\contact_mgmt_ddl.sql"
    Write-Output ".\psql_shell.ps1 flex dev psql\contact_mgmt_ddl.sql"
    Write-Output ".\psql_shell.ps1 cosmos citus psql\contact_mgmt_ddl.sql"
    Write-Output ""
    Exit
}

if ("" -eq $psql_file) {
    Write-Output "connecting to host: $h, db: $db_name, user: $user"
    $psql_args="host=$h port=$p dbname=$db_name user=$user password=$pass $ssl"
    Write-Output $psql_args
    psql "$psql_args"
}
else {
    Write-Output "connecting to host: $h, db: $db_name, user: $user, using file: $psql_file"
    $psql_args="host=$h port=$p dbname=$db_name user=$user password=$pass $ssl"
    psql -f $psql_file "$psql_args"
}
