// db/connect.go
package db

import (
    "database/sql"
    "log"
    "os"

    _ "github.com/lib/pq"
)

var DB *sql.DB

func Init() {
    var err error
    dsn := os.Getenv("DATABASE_URL")
    DB, err = sql.Open("postgres", dsn)
    if err != nil {
        log.Fatalf("Failed to open DB: %v", err)
    }

    if err = DB.Ping(); err != nil {
        log.Fatalf("Failed to connect to DB: %v", err)
    }

    log.Println("✅ Database connected")
}
