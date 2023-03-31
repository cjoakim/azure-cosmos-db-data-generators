
DROP TABLE IF EXISTS companies CASCADE;

# "id,company_id,name,org_type,hq_state,memo,created_on,created_by,modified_on,modified_by,expiration_date,is_deleted";
 
CREATE TABLE companies (
    id          UUID NOT NULL,
    company_id  INT NOT NULL,
    name        VARCHAR(255) NOT NULL,
    org_type    VARCHAR(16) NOT NULL,
    hq_state    VARCHAR(4) NOT NULL,
    memo        VARCHAR(255) NOT NULL,

    created_on  timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by  VARCHAR(50) NOT NULL,
    modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_by VARCHAR(50) NOT NULL,
    expiration_date timestamp NULL,
    is_deleted  BOOLEAN NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);

CREATE TABLE contacts (
    id          UUID NOT NULL,
    company_id  INT NOT NULL,
    name        VARCHAR(255) NOT NULL,
    org_type    VARCHAR(16) NOT NULL,
    hq_state    VARCHAR(4) NOT NULL,
    memo        VARCHAR(255) NOT NULL,

    created_on  timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by  VARCHAR(50) NOT NULL,
    modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_by VARCHAR(50) NOT NULL,
    expiration_date timestamp NULL,
    is_deleted  BOOLEAN NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);

CREATE TABLE contact_methods (
    id          UUID NOT NULL,
    company_id  INT NOT NULL,
    name        VARCHAR(255) NOT NULL,
    org_type    VARCHAR(16) NOT NULL,
    hq_state    VARCHAR(4) NOT NULL,
    memo        VARCHAR(255) NOT NULL,

    created_on  timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by  VARCHAR(50) NOT NULL,
    modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_by VARCHAR(50) NOT NULL,
    expiration_date timestamp NULL,
    is_deleted  BOOLEAN NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);



\d companies
\copy companies from /Users/chjoakim/github/azure-cosmos-db-data-generators/dotnet/ContactManagement/data/companies.csv csv header;
\x
select * from companies offset 0 limit 1;

