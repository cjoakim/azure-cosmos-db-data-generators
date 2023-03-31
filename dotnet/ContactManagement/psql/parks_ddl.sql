
-- generated from file: data/lahman_baseball/baseballdatabank-2023.1/core/Parks.csv

DROP TABLE IF EXISTS parks CASCADE;

CREATE TABLE parks (
    id                   SERIAL,
    park_key             VARCHAR(255),
    park_name            VARCHAR(255),
    park_alias           VARCHAR(255),
    city                 VARCHAR(255),
    state                VARCHAR(255),
    country              VARCHAR(255)
);

\d parks
\copy parks(park_key,park_name,park_alias,city,state,country) from /Users/chjoakim/github/azure-cosmosdb-pg0/data/lahman_baseball/baseballdatabank-2023.1/core/Parks.csv csv header;
\x
select * from parks offset 0 limit 3;

