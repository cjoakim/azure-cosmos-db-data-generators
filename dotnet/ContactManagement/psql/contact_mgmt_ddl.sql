
DROP TABLE IF EXISTS companies CASCADE;
DROP TABLE IF EXISTS contacts CASCADE;
DROP TABLE IF EXISTS contact_methods CASCADE;

CREATE TABLE companies (
    id                       UUID NOT NULL,
    company_id               INT NOT NULL,
    name                     VARCHAR(255) NOT NULL,
    org_type                 VARCHAR(16) NOT NULL,
    hq_state                 VARCHAR(4) NOT NULL,
    memo                     VARCHAR(255) NOT NULL,
    created_on               timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by               VARCHAR(50) NOT NULL,
    modified_on              timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_by              VARCHAR(50) NOT NULL,
    expiration_date          timestamp NULL,
    is_deleted               BOOLEAN NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);

\d companies

CREATE TABLE contacts (
    id                       UUID NOT NULL,
    company_id               INT NOT NULL,
    contact_id               INT NOT NULL,
    name                     VARCHAR(255) NOT NULL,
    preferred_contact_method VARCHAR(8) NOT NULL,
    roles                    VARCHAR(255) NOT NULL,
    notification_references  VARCHAR(255) NOT NULL,
    created_on               timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by               VARCHAR(50) NOT NULL,
    modified_on              timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_by              VARCHAR(50) NOT NULL,
    expiration_date          timestamp NULL,
    is_deleted               BOOLEAN NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);

\d contacts

CREATE TABLE contact_methods (
    id                       UUID NOT NULL,
    company_id               INT NOT NULL,
    contact_id               INT NOT NULL,
    type                     VARCHAR(16) NOT NULL,
    value                    VARCHAR(255) NOT NULL,
    memo                     VARCHAR(255) NOT NULL,
    created_on               timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by               VARCHAR(50) NOT NULL,
    modified_on              timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_by              VARCHAR(50) NOT NULL,
    expiration_date          timestamp NULL,
    is_deleted               BOOLEAN NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);

\d contact_methods

-- Load the tables
\x

\copy companies from /Users/chjoakim/github/azure-cosmos-db-data-generators/dotnet/ContactManagement/data/companies.csv csv header;
select * from companies offset 0 limit 1;

\copy contacts  from /Users/chjoakim/github/azure-cosmos-db-data-generators/dotnet/ContactManagement/data/contacts.csv csv header;
select * from contacts offset 0 limit 1;

\copy contact_methods from /Users/chjoakim/github/azure-cosmos-db-data-generators/dotnet/ContactManagement/data/contact_methods.csv csv header;
select * from contact_methods offset 0 limit 1;
