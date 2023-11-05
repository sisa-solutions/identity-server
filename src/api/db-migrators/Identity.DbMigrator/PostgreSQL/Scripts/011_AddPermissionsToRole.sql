START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM migrations WHERE "id" = '20231105085842_AddPermissionsToRole') THEN
    ALTER TABLE roles ADD permissions text[] NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM migrations WHERE "id" = '20231105085842_AddPermissionsToRole') THEN
    ALTER TABLE applications ALTER COLUMN settings SET DEFAULT ('{}');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM migrations WHERE "id" = '20231105085842_AddPermissionsToRole') THEN
    ALTER TABLE applications ALTER COLUMN json_web_key_set SET DEFAULT ('{}');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM migrations WHERE "id" = '20231105085842_AddPermissionsToRole') THEN
    INSERT INTO migrations (id, version)
    VALUES ('20231105085842_AddPermissionsToRole', '8.0.0-rc.2.23480.1');
    END IF;
END $EF$;
COMMIT;

