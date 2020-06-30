CREATE TABLE "user"
(
    user_id		UUID PRIMARY KEY,
    username	VARCHAR(30) NOT NULL,
	created_at 	TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "badge" (
	badge_id   			UUID PRIMARY KEY,
    badge_name 			VARCHAR(30) NOT NULL,
    badge_description	VARCHAR(100) NOT NULL,
	user_id				UUID NOT NULL,
    created_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES "user" (user_id)
)

CREATE TABLE "habit"
(
    habit_id	UUID PRIMARY KEY,
    habit_name	VARCHAR(50) NOT NULL,
	days_off	text[] NULL,
	user_id		UUID NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "user" (user_id),
	created_at 	TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);