
param(
    [Parameter()]
    [String]$env_name  = "<env>",
    [String]$db_name   = "<db>"
)

# Usage:
# .\delete_defile_load_contact_mgmt.ps1 <env> <dbname>
# .\delete_defile_load_contact_mgmt.ps1 local dev

Write-Output "companies..."
.\psql.ps1 $env_name $db_name psql\contact_mgmt_ddl.sql



