CREATE TABLE IF NOT EXISTS "public"."collection" (
  "id" uuid NOT NULL,
  "external_id" varchar(255),
  "icon_id" uuid,
  "name" varchar(255) NOT NULL,
  "status" int4 NOT NULL,
  "date_added" timestamp NOT NULL,
  PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "public"."collection_item" (
  "id" uuid NOT NULL,
  "collection_id" uuid NOT NULL,
  "external_id" varchar(255),
  "icon_id" uuid,
  "name" varchar(255) NOT NULL,
  PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "public"."icon" (
  "id" uuid NOT NULL,
  "external_id" varchar(255),
  "title" varchar(255),
  "binary" bytea NOT NULL,
  PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "public"."material" (
  "id" uuid NOT NULL,
  "external_id" varchar(255),
  "icon_id" uuid,
  "name" varchar(255) NOT NULL,
  "type" int4 NOT NULL,
  "status" int4 NOT NULL,
  "date_added" timestamp NOT NULL,
  PRIMARY KEY ("id")
);

ALTER TABLE IF EXISTS "public"."collection_item" DROP CONSTRAINT IF EXISTS "fk_collection_item_collection_1";
ALTER TABLE IF EXISTS "public"."collection_item" ADD CONSTRAINT "fk_collection_item_collection_1" FOREIGN KEY ("collection_id") REFERENCES "public"."collection" ("id");
