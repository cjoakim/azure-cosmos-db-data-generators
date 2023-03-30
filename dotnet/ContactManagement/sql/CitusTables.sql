/*SELECT * FROM pg_available_extensions;*/

/*SELECT * FROM pg_constraint;*/

/*SELECT SUBSTRING(current_user, 2)*/

/*DELETE FROM cm.contacts;*/
/*SELECT c.*, cm.*
FROM cm.contacts AS c
JOIN cm.contact_methods AS cm
ON c.tenant_id = cm.tenant_id AND c.id = cm.contact_id
WHERE c.tenant_id = '1701160d594544b8849c6ec9834e3bb4'
AND c.name LIKE 'Test%'
AND cm.value LIKE 'Test%';*/

/*Cleanup*/
SELECT cron.unschedule('hard_delete_cm.tenants');
SELECT cron.unschedule('hard_delete_cm.contacts');
SELECT cron.unschedule('hard_delete_cm.contact_methods');
ALTER TABLE cm.contacts DROP CONSTRAINT contacts_tenant_id_prefered_contact_method_id_fkey;
DROP TABLE cm.contact_methods;
DROP TABLE cm.contacts;
DROP TABLE cm.tenants;
DROP SCHEMA cm;


/*Contact Managment*/
CREATE SCHEMA cm;
GRANT USAGE ON SCHEMA cm TO t1701160d594544b8849c6ec9834e3bb4, t4b0cb5a5a3cc442ead9c3b662a77e907;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA cm TO t1701160d594544b8849c6ec9834e3bb4, t4b0cb5a5a3cc442ead9c3b662a77e907;


/*Tenant*/
CREATE TABLE cm.tenants (
    id UUID NOT NULL,
    created_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by VARCHAR(50) NOT NULL,
    last_modified_by VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    expiration_date date NULL,
    is_deleted boolean NOT NULL DEFAULT false,
    PRIMARY KEY(id)
);
CREATE INDEX idx_tenant_expiration_date_is_deleted ON cm.tenants(expiration_date, is_deleted);
CREATE INDEX idx_tenant_name__text ON cm.tenants(name text_pattern_ops);
CREATE POLICY admin_policy ON cm.tenants
  TO citus
  USING (true)
  WITH CHECK (true);
CREATE POLICY user_policy ON cm.tenants
  USING (id = UUID(SUBSTRING(current_user, 2)));
ALTER TABLE cm.tenants ENABLE ROW LEVEL SECURITY;
GRANT select, update, insert, delete
  ON cm.tenants TO t1701160d594544b8849c6ec9834e3bb4, t4b0cb5a5a3cc442ead9c3b662a77e907;
SELECT create_distributed_table('cm.tenants', 'id');
SELECT cron.schedule('hard_delete_cm.tenants', '0 10 * * *', $$DELETE FROM cm.tenants WHERE expiration_date >= CURRENT_DATE AND is_deleted = true$$);


/*Contact*/
CREATE TABLE cm.contacts (
    tenant_id UUID NOT NULL,
    id UUID NOT NULL,
    created_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by VARCHAR(50) NOT NULL,
    last_modified_by VARCHAR(50) NOT NULL,
    reference_id VARCHAR(50) NOT NULL,
    prefered_contact_method_id UUID NULL,
    role VARCHAR(50) NOT NULL,
    name VARCHAR(500) NOT NULL,
    expiration_date date NULL,
    is_deleted boolean NOT NULL DEFAULT false,
    PRIMARY KEY(tenant_id, id),
    UNIQUE(tenant_id, reference_id),
    FOREIGN KEY(tenant_id) REFERENCES cm.tenants ON DELETE CASCADE
);

CREATE INDEX idx_contact_expiration_date_is_deleted ON cm.contacts(expiration_date, is_deleted);
CREATE INDEX idx_contact_tenant_id_name__text ON cm.contacts(tenant_id, name text_pattern_ops);
CREATE POLICY admin_policy ON cm.contacts
  TO citus
  USING (true)
  WITH CHECK (true);
CREATE POLICY user_policy ON cm.contacts
  USING (tenant_id = UUID(SUBSTRING(current_user, 2)));
ALTER TABLE cm.contacts ENABLE ROW LEVEL SECURITY;
GRANT select, update, insert, delete
  ON cm.contacts TO t1701160d594544b8849c6ec9834e3bb4, t4b0cb5a5a3cc442ead9c3b662a77e907;
SELECT create_distributed_table('cm.contacts', 'tenant_id', colocate_with => 'cm.tenants');
/*Run in more secure contaxt ueser*/
/*Move data to old table instead of delete*/
/*use utc date*/
SELECT cron.schedule('hard_delete_cm.contacts', '0 10 * * *', $$DELETE FROM cm.contacts WHERE expiration_date >= CURRENT_DATE AND is_deleted = true$$);


/*Contact Method*/
CREATE TABLE cm.contact_methods (
    tenant_id UUID NOT NULL,
    id UUID NOT NULL,
    created_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    modified_on timestamp without time zone NOT NULL DEFAULT timezone('utc', now()),
    created_by VARCHAR(50) NOT NULL,
    last_modified_by VARCHAR(50) NOT NULL,
    contact_id UUID NOT NULL,
    type VARCHAR(50) NOT NULL,
    value VARCHAR(4000) NOT NULL,
    memo VARCHAR(255) NULL,
    expiration_date date NULL,
    is_deleted boolean NOT NULL DEFAULT false,
    PRIMARY KEY(tenant_id, id),
    FOREIGN KEY(tenant_id, contact_id) REFERENCES cm.contacts ON DELETE CASCADE
);

CREATE INDEX idx_contact_method_expiration_date_is_deleted ON cm.contact_methods(expiration_date, is_deleted);
CREATE INDEX idx_contact_method_tenant_id_contact_id_type ON cm.contact_methods(tenant_id, contact_id, type);
CREATE INDEX idx_contact_method_tenant_id_value__text ON cm.contact_methods(tenant_id, value text_pattern_ops);
CREATE INDEX idx_contact_method_tenant_id_memo__text ON cm.contact_methods(tenant_id, memo text_pattern_ops);
CREATE POLICY admin_policy ON cm.contact_methods
  TO citus
  USING (true)
  WITH CHECK (true);
CREATE POLICY user_policy ON cm.contact_methods
  USING (tenant_id = UUID(SUBSTRING(current_user, 2)));
ALTER TABLE cm.contact_methods ENABLE ROW LEVEL SECURITY;
GRANT select, update, insert, delete
  ON cm.contact_methods TO t1701160d594544b8849c6ec9834e3bb4, t4b0cb5a5a3cc442ead9c3b662a77e907;
SELECT create_distributed_table('cm.contact_methods', 'tenant_id', colocate_with => 'cm.contacts');
SELECT cron.schedule('hard_delete_cm.contact_methods', '0 10 * * *', $$DELETE FROM cm.contact_methods WHERE expiration_date >= CURRENT_DATE AND is_deleted = true$$);


/*Contact Fogeign Key Update*/
ALTER TABLE cm.contacts ADD FOREIGN KEY(tenant_id, prefered_contact_method_id)
REFERENCES cm.contact_methods ON DELETE RESTRICT;


/*Tenant 1 Data*/
INSERT INTO cm.tenants (id, created_by, last_modified_by, name)
VALUES ('1701160d-5945-44b8-849c-6ec9834e3bb4', 'Test User', 'Test User', 'Test Tenant 1');

INSERT INTO cm.contacts (tenant_id, id, created_by, last_modified_by, reference_id, prefered_contact_method_id, role, name)
VALUES ('1701160d-5945-44b8-849c-6ec9834e3bb4', '439bac59-cad1-481a-b7ab-6a1b941b68c3', 'Test User', 'Test User', '1', NULL, 'Test Role', 'Test Contact');

INSERT INTO cm.contact_methods (tenant_id, id, created_by, last_modified_by, contact_id, type, value, memo)
VALUES ('1701160d-5945-44b8-849c-6ec9834e3bb4', '8820e287-b498-4559-9172-80eb28d9b45b', 'Test User', 'Test User', '439bac59-cad1-481a-b7ab-6a1b941b68c3', 'Test Type', 'Test Value', 'Test Memo');

UPDATE cm.contacts
SET prefered_contact_method_id = '8820e287-b498-4559-9172-80eb28d9b45b', modified_on = timezone('utc', now())
WHERE tenant_id = '1701160d-5945-44b8-849c-6ec9834e3bb4' AND id = '439bac59-cad1-481a-b7ab-6a1b941b68c3';


/*Tenant 2 Data*/
INSERT INTO cm.tenants (id, created_by, last_modified_by, name)
VALUES ('4b0cb5a5-a3cc-442e-ad9c-3b662a77e907', 'Test User', 'Test User', 'Test Tenant 2');

INSERT INTO cm.contacts (tenant_id, id, created_by, last_modified_by, reference_id, prefered_contact_method_id, role, name)
VALUES ('4b0cb5a5-a3cc-442e-ad9c-3b662a77e907', '68dd0df7-2ed7-4125-87d9-709bd8b7cfd1', 'Test User', 'Test User', '1', NULL, 'Test Role', 'Test Contact');

INSERT INTO cm.contact_methods (tenant_id, id, created_by, last_modified_by, contact_id, type, value, memo)
VALUES ('4b0cb5a5-a3cc-442e-ad9c-3b662a77e907', '984e1d04-11ee-48b7-b176-e392b8b9aebb', 'Test User', 'Test User', '68dd0df7-2ed7-4125-87d9-709bd8b7cfd1', 'Test Type', 'Test Value', 'Test Memo');

UPDATE cm.contacts
SET prefered_contact_method_id = '984e1d04-11ee-48b7-b176-e392b8b9aebb', modified_on = timezone('utc', now())
WHERE tenant_id = '4b0cb5a5-a3cc-442e-ad9c-3b662a77e907' AND id = '68dd0df7-2ed7-4125-87d9-709bd8b7cfd1';


/*Sample Join*/
SELECT c.*, cm.*
FROM cm.contacts AS c
JOIN cm.contact_methods AS cm
ON c.tenant_id = cm.tenant_id AND c.id = cm.contact_id
/*WHERE c.tenant_id = '1701160d-5945-44b8-849c-6ec9834e3bb4'*/
AND c.name LIKE 'Test%'
AND cm.value LIKE 'Test%'
