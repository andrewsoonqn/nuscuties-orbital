package models

type Quest struct {
    ID         int       `json:"id"`
    UserID     string    `json:"user_id"`
    Title      string    `json:"title"`
    Completed  bool      `json:"completed"`
    ExpReward  int       `json:"exp_reward"`
}
