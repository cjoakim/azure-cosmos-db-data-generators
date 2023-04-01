# PowerShell script to delete, define, and load a target
# PostgreSQL database with the CSV files generated in this project.
#
# Chris Joakim, Microsoft, 2023

param(
    [Parameter()]
    [String]$env_name  = "<env>",
    [String]$db_name   = "<db>"
)

# Usage:
# .\delete_defile_load_contact_mgmt.ps1 <env> <dbname>
# .\delete_defile_load_contact_mgmt.ps1 local dev

Write-Output "companies..."
.\psql_shell.ps1 $env_name $db_name psql\contact_mgmt_ddl.sql
