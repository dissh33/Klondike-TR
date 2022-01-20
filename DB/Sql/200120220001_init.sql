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

CREATE TABLE IF NOT EXISTS "public"."offer" (
  "id" uuid NOT NULL,
  "user_id" varchar(255) NOT NULL,
  "name" varchar(255),
  "type" int4 NOT NULL,
  "status" int4 NOT NULL,
  "create_date" timestamp NOT NULL,
  "update_date" timestamp NOT NULL,
  "is_deleted" bool NOT NULL,
  "delete_date" timestamp,
  PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "public"."offer_item" (
  "id" uuid NOT NULL,
  "offer_position_id" uuid NOT NULL,
  "tradable_item_id" uuid NOT NULL,
  "type" int4 NOT NULL,
  "number" int4 NOT NULL,
  PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "public"."offer_position" (
  "id" uuid NOT NULL,
  "offer_id" uuid NOT NULL,
  "type" int4 NOT NULL,
  "price_rate_string" varchar(255),
  PRIMARY KEY ("id")
);

ALTER TABLE IF EXISTS "public"."collection_item" DROP CONSTRAINT IF EXISTS "fk_collection_item_collection_1";
ALTER TABLE IF EXISTS "public"."offer_item" DROP CONSTRAINT IF EXISTS "fk_offer_item_offer_position_1";
ALTER TABLE IF EXISTS "public"."offer_position" DROP CONSTRAINT IF EXISTS "fk_offer_position_offer_1";

ALTER TABLE IF EXISTS "public"."collection_item" ADD CONSTRAINT "fk_collection_item_collection_1" FOREIGN KEY ("collection_id") REFERENCES "public"."collection" ("id");
ALTER TABLE IF EXISTS "public"."offer_item" ADD CONSTRAINT "fk_offer_item_offer_position_1" FOREIGN KEY ("offer_position_id") REFERENCES "public"."offer_position" ("id");
ALTER TABLE IF EXISTS "public"."offer_position" ADD CONSTRAINT "fk_offer_position_offer_1" FOREIGN KEY ("offer_id") REFERENCES "public"."offer" ("id");

