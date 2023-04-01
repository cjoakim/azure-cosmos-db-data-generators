# azure-cosmos-db-data-generators : DotNet

---

## Contact Management for PostgreSQL

### Clone this repository

```
> git clone https://github.com/cjoakim/azure-cosmos-db-data-generators.git
```

### Navigate and Compile

```
> cd .\dotnet\
> cd .\ContactManagement\

> dotnet --version
7.0.202

> dotnet build
```

### Generate CSV Data for loading into PostgreSQL

The DotNet Bogus and Faker.Net libraries are used, and some randomness
is added such that the output row counts will vary for each execution.

```
> mkdir data

> dotnet run generate csv
...
File written: data/companies.csv
...
File written: data/contacts.csv
...
File written: data/contact_methods.csv
...
```

### Modify the Data-Generation Code as Necessary

For more or less data, modify class DataGenerator.

```
    public class DataGenerator
    {
        // Parameters: Companies
        private static int smallBusinessCount = 1000;
        private static int corporationCount   =  100;

        // Parameters: Contacts
        private static int maxContactsPerSmallBusiness =  3;
        private static int maxContactsPerCorporation   = 12;
    ...
```

To create CSV files with additional attributes, see the Model classes 
(i.e. - Company, Contact, ContactMethod) where attributes are defined and CSV is rendered,
and modify these as necessary.  Also modify class DataGenerator to populate
the additional attributes.

### Optionally Load the CSV into a PostgreSQL database

The database can be on localhost, an Azure PostgreSQL Flexible Server,
or Azure Cosmos DB for PostgreSQL.

See **psql_shell.ps1**, and set the following environment variables accordingly:

```
# For localhost PostgreSQL
LOCAL_PG_USER
LOCAL_PG_PASS

# For Azure PostgreSQL Flexible Server
AZURE_PG_SERVER_FULL_NAME
AZURE_PG_USER
AZURE_PG_PASS

# Azure Cosmos DB for PostgreSQL
AZURE_COSMOSDB_PG_SERVER_FULL_NAME
AZURE_COSMOSDB_PG_USER
AZURE_COSMOSDB_PG_PASS
```

See the command line options to **psql_shell.ps1**

```
> .\psql_shell.ps1

Usage:
.\psql_shell.ps1 <env> <db>  where env is local, flex, or cosmos
.\psql_shell.ps1 <env> <db> <infile>
.\psql_shell.ps1 local dev
.\psql_shell.ps1 flex dev
.\psql_shell.ps1 cosmos citus
.\psql_shell.ps1 local dev psql\contact_mgmt_ddl.sql
.\psql_shell.ps1 flex dev psql\contact_mgmt_ddl.sql
.\psql_shell.ps1 cosmos citus psql\contact_mgmt_ddl.sql
```

**See file 'psql\contact_mgmt_ddl.sql'.  This DDL is only intended to demonstrate**
**that the generated CSV can be successfully loaded to PostgreSQL.  There is no indexing**
**or Cosmos DB distributed table creation.**

**Modify file 'psql\contact_mgmt_ddl.sql' regarding the input CSV file paths.**
**Replace the '/Users/chjoakim/.../data/' path with your filesystem location.**

Load localhost dev database.

```
> .\delete_defile_load_contact_mgmt.ps1 local dev

companies...
connecting to host: localhost, db: dev, user: chjoakim, using file: psql\contact_mgmt_ddl.sql
DROP TABLE
DROP TABLE
DROP TABLE
CREATE TABLE
                                      Table "public.companies"
     Column      |            Type             | Collation | Nullable |           Default
-----------------+-----------------------------+-----------+----------+------------------------------
 id              | uuid                        |           | not null |
 company_id      | integer                     |           | not null |
 name            | character varying(255)      |           | not null |
 org_type        | character varying(16)       |           | not null |
 hq_state        | character varying(4)        |           | not null |
 memo            | character varying(255)      |           | not null |
 created_on      | timestamp without time zone |           | not null | timezone('utc'::text, now())
 created_by      | character varying(50)       |           | not null |
 modified_on     | timestamp without time zone |           | not null | timezone('utc'::text, now())
 modified_by     | character varying(50)       |           | not null |
 expiration_date | timestamp without time zone |           |          |
 is_deleted      | boolean                     |           | not null | false
Indexes:
    "companies_pkey" PRIMARY KEY, btree (id)


CREATE TABLE
                                           Table "public.contacts"
          Column          |            Type             | Collation | Nullable |           Default
--------------------------+-----------------------------+-----------+----------+------------------------------
 id                       | uuid                        |           | not null |
 company_id               | integer                     |           | not null |
 contact_id               | integer                     |           | not null |
 name                     | character varying(255)      |           | not null |
 preferred_contact_method | character varying(8)        |           | not null |
 roles                    | character varying(255)      |           | not null |
 notification_references  | character varying(255)      |           | not null |
 created_on               | timestamp without time zone |           | not null | timezone('utc'::text, now())
 created_by               | character varying(50)       |           | not null |
 modified_on              | timestamp without time zone |           | not null | timezone('utc'::text, now())
 modified_by              | character varying(50)       |           | not null |
 expiration_date          | timestamp without time zone |           |          |
 is_deleted               | boolean                     |           | not null | false
Indexes:
    "contacts_pkey" PRIMARY KEY, btree (id)


CREATE TABLE
                                   Table "public.contact_methods"
     Column      |            Type             | Collation | Nullable |           Default
-----------------+-----------------------------+-----------+----------+------------------------------
 id              | uuid                        |           | not null |
 company_id      | integer                     |           | not null |
 contact_id      | integer                     |           | not null |
 type            | character varying(16)       |           | not null |
 value           | character varying(255)      |           | not null |
 memo            | character varying(255)      |           | not null |
 created_on      | timestamp without time zone |           | not null | timezone('utc'::text, now())
 created_by      | character varying(50)       |           | not null |
 modified_on     | timestamp without time zone |           | not null | timezone('utc'::text, now())
 modified_by     | character varying(50)       |           | not null |
 expiration_date | timestamp without time zone |           |          |
 is_deleted      | boolean                     |           | not null | false
Indexes:
    "contact_methods_pkey" PRIMARY KEY, btree (id)


Expanded display is on.
COPY 1100
-[ RECORD 1 ]---+-------------------------------------
id              | 1b5f6dfa-8fbe-455a-80fa-44929cc8a2e0
company_id      | 1000002
name            | Armstrong and Sons LLC
org_type        | small_business
hq_state        | FL
memo            |
created_on      | 2023-04-01 05:53:16
created_by      | migration
modified_on     | 2023-04-01 05:53:16
modified_by     | migration
expiration_date |
is_deleted      | f


COPY 120607
-[ RECORD 1 ]------------+-------------------------------------
id                       | a67e08b8-967f-4b89-bc30-9dc03d9804ac
company_id               | 1000002
contact_id               | 1002201
name                     | Angelina Jacobs 1
preferred_contact_method | email
roles                    | Owner
notification_references  | Invoices
created_on               | 2023-04-01 05:53:20
created_by               | migration
modified_on              | 2023-04-01 05:53:20
modified_by              | migration
expiration_date          |
is_deleted               | f


COPY 120607
-[ RECORD 1 ]---+-------------------------------------
id              | 3f5bb3aa-3c8a-4b97-8db3-697c1fd024f1
company_id      | 1000002
contact_id      | 1002201
type            | email
value           | Milford.Ankunding@hotmail.com
memo            | As of 4/1/2023
created_on      | 2023-04-01 05:54:31
created_by      | migration
modified_on     | 2023-04-01 05:54:31
modified_by     | migration
expiration_date |
is_deleted      | f
```

### Sample Queries

```
SELECT
    con.company_id, con.name, ctc.contact_id, ctc.name
FROM
    companies as con
INNER JOIN contacts as ctc
    ON con.company_id = ctc.company_id
where con.company_id in (1000002, 1000004)
order by con.company_id, ctc.name;
```

```
SELECT
    *
FROM
    contacts
INNER JOIN contact_methods
    ON  contacts.company_id = contact_methods.company_id
    AND contacts.contact_id = contact_methods.contact_id;
```
