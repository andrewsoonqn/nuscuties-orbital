package handlers

import (
    "database/sql"
    "encoding/json"
    "fmt"
    "net/http"
    "strconv"
    "strings"
    "nuscuties-backend/models"
)

var DB *sql.DB

func SetDB(conn *sql.DB) {
    DB = conn
}

// GET /quests
func GetQuests(w http.ResponseWriter, r *http.Request) {
    rows, err := DB.Query("SELECT id, title, description, completed FROM quests")
    if err != nil {
        http.Error(w, err.Error(), http.StatusInternalServerError)
        return
    }
    defer rows.Close()

    var quests []models.Quest
    for rows.Next() {
        var q models.Quest
        if err := rows.Scan(&q.ID, &q.Title, &q.Description, &q.Completed); err != nil {
            http.Error(w, err.Error(), http.StatusInternalServerError)
            return
        }
        quests = append(quests, q)
    }

    w.Header().Set("Content-Type", "application/json")
    json.NewEncoder(w).Encode(quests)
}

// POST /quests
func CreateQuest(w http.ResponseWriter, r *http.Request) {
    var q models.Quest
    if err := json.NewDecoder(r.Body).Decode(&q); err != nil {
        http.Error(w, err.Error(), http.StatusBadRequest)
        return
    }

    _, err := DB.Exec("INSERT INTO quests (title, description, completed) VALUES ($1, $2, $3)",
        q.Title, q.Description, q.Completed)
    if err != nil {
        http.Error(w, err.Error(), http.StatusInternalServerError)
        return
    }

    w.WriteHeader(http.StatusCreated)
    fmt.Fprint(w, "Quest created")
}

// PUT /quests/{id}/complete
func CompleteQuest(w http.ResponseWriter, r *http.Request) {
    // Extract quest ID from URL path
    parts := strings.Split(r.URL.Path, "/")
    if len(parts) < 3 {
        http.Error(w, "Invalid request", http.StatusBadRequest)
        return
    }
    id, err := strconv.Atoi(parts[2])
    if err != nil {
        http.Error(w, "Invalid quest ID", http.StatusBadRequest)
        return
    }

    // Mark the quest as completed
    _, err = DB.Exec("UPDATE quests SET completed=true WHERE id=$1", id)
    if err != nil {
        http.Error(w, err.Error(), http.StatusInternalServerError)
        return
    }

    fmt.Fprint(w, "Quest marked as complete")
}
