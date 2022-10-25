ALTER TABLE IF EXISTS "public"."offer_position"
	ALTER COLUMN "with_trader" TYPE bool USING "with_trader"::boolean
