CREATE TABLE "user"
(
    user_id		UUID PRIMARY KEY,
    username	VARCHAR(30) NOT NULL,
	created_at 	TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "habit"
(
    habit_id	UUID PRIMARY KEY,
    habit_name	VARCHAR(50) NOT NULL,
	days_off	text[] NULL,
	user_id		UUID NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "user" (user_id)
	ON DELETE CASCADE ON UPDATE CASCADE,
	created_at 	TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "habit_logs"
(
	logs_id			UUID PRIMARY KEY,
	habit_id		UUID NOT NULL,
	user_id			UUID NOT NULL,
	logs_created 	TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (habit_id) REFERENCES "habit" (habit_id)
	ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (user_id) REFERENCES "user" (user_id)
	ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE habit_logs_snapshot
(
	logs_snapshot_id		UUID PRIMARY KEY,
	last_habit_id			UUID NOT NULL,
	last_user_id			UUID NOT NULL,
	last_streak				INT NOT NULL,
	logs_snapshot_created	TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (last_habit_id) REFERENCES "habit" (habit_id)
	ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (last_user_id) REFERENCES "user" (user_id)
	ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE "badge" (
	badge_id   			UUID PRIMARY KEY,
    badge_name 			VARCHAR(30) NOT NULL,
    badge_description	VARCHAR(100) NOT NULL
)

CREATE TABLE "badge_user"
(
	badge_id			UUID PRIMARY KEY,
	user_id				UUID NOT NULL,
    created_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (badge_id) REFERENCES "badge" (badge_id),
    FOREIGN KEY (user_id) REFERENCES "user" (user_id)
)

-- INSERT INTO badge VALUES 
-- ('faae7518-2bc1-40cd-8dfd-a2acb47ec82b',
-- 'Dominating',
-- '4+ streak')

-- INSERT INTO badge VALUES
-- ('18c079f8-590d-4ad8-9b69-7e364d058826',
-- 'Workaholic',
-- 'Doing some works on days-off')

-- INSERT INTO badge VALUES
-- ('0a4b9805-103a-41e7-ba0a-724531ec7c9d',
-- 'Epic Comeback',
-- '10 streak after 10 days without logging')