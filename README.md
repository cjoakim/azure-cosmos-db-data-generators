# azure-cosmos-db-data-generators

A collection of data generator utilities for Cosmos DB - NoSQL, Mongo, and PostgreSQL APIs.

---

## Datasets

- https://www.kaggle.com/
- https://www.imdb.com/interfaces/
- https://datasets.imdbws.com/
- https://openflights.org/data.html
- https://www.seanlahman.com/baseball-archive/

### Libraries

- Python https://faker.readthedocs.io/en/master/
- DotNet https://www.nuget.org/packages/Faker.Net/
- DotNet https://www.nuget.org/packages/Bogus

### Tools

- https://synthetichealth.github.io/synthea/


---

## Sample PostgreSQL Queries

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