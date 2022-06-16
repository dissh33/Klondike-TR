ALTER TABLE IF EXISTS "public"."offer_item" DROP CONSTRAINT "fk_offer_item_offer_position_1" CASCADE;
ALTER TABLE IF EXISTS "public"."offer_position" DROP CONSTRAINT "fk_offer_position_offer_1" CASCADE;

DROP TABLE IF EXISTS "public"."offer" CASCADE;
DROP TABLE IF EXISTS "public"."offer_item" CASCADE;
DROP TABLE IF EXISTS "public"."offer_position" CASCADE;

CREATE TABLE "public"."offer" (
  "id" uuid NOT NULL,
  "user_id" varchar(255) NOT NULL,
  "title" varchar(255),
  "type" int4 NOT NULL,
  "status" int4 NOT NULL,
  "message" text,
  "expression" jsonb NOT NULL,
  "create_date" timestamp NOT NULL,
  "update_date" timestamp NOT NULL,
  "is_deleted" bool NOT NULL,
  "delete_date" timestamp,
  PRIMARY KEY ("id")
);

CREATE TABLE "public"."offer_item" (
  "id" uuid NOT NULL,
  "offer_position_id" uuid NOT NULL,
  "tradable_item_id" uuid NOT NULL,
  "type" int4 NOT NULL,
  "amount" int4 NOT NULL,
  "create_date" timestamp NOT NULL,
  "update_date" timestamp NOT NULL,
  "is_deleted" bool NOT NULL,
  "delete_date" timestamp,
  PRIMARY KEY ("id")
);

CREATE TABLE "public"."offer_position" (
  "id" uuid NOT NULL,
  "offer_id" uuid NOT NULL,
  "type" int4 NOT NULL,
  "price_rate" varchar(255),
  "with_trader" int4 NOT NULL,
  "message" text,
  "create_date" timestamp NOT NULL,
  "update_date" timestamp NOT NULL,
  "is_deleted" bool NOT NULL,
  "delete_date" timestamp,
  PRIMARY KEY ("id")
);

ALTER TABLE "public"."offer_item" ADD CONSTRAINT "fk_offer_item_offer_position_1" FOREIGN KEY ("offer_position_id") REFERENCES "public"."offer_position" ("id");
ALTER TABLE "public"."offer_position" ADD CONSTRAINT "fk_offer_position_offer_1" FOREIGN KEY ("offer_id") REFERENCES "public"."offer" ("id");