ALTER TABLE IF EXISTS "public"."offer_position"
	ALTER COLUMN "with_trader" TYPE int4 USING "with_trader"::integer
