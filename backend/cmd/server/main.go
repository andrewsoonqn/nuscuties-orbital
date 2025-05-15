package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
	"strings"

    	"github.com/joho/godotenv"
    	"nuscuties-backend/db"
	"nuscuties-backend/handlers"
	
)

func main() {
	_ = godotenv.Load() // load .env file
	
	fmt.Println("Starting backend server...")

	db.Init() // connect to db
	handlers.SetDB(db.DB) // share db connection with handlers

	// Example: Basic HTTP server setup (you'll expand this later)
	// We'll define handlers elsewhere later
	// http.HandleFunc("/", handleRoot) // Example placeholder

	port := os.Getenv("PORT") // Get port from environment variable
	if port == "" {
		port = "8080" // Default port if not set
		fmt.Printf("Defaulting to port %s\n", port)
	}

	log.Printf("Listening on port %s\n", port)
	if err := http.ListenAndServe(":"+port, nil); err != nil {
        	log.Fatal(err)
	}

	// get and post quests
	http.HandleFunc("/quests", func(w http.ResponseWriter, r *http.Request) {
	    switch r.Method {
	    case "GET":
	        handlers.GetQuests(w, r)
	    case "POST":
	        handlers.CreateQuest(w, r)
	    default:
	        http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
	    }
})

	http.HandleFunc("/quests/", func(w http.ResponseWriter, r *http.Request) {
    	if r.Method == "PUT" && strings.HasSuffix(r.URL.Path, "/complete") {
        	handlers.CompleteQuest(w, r)
        	return
    	}
    		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
	})
}

// Example placeholder handler function (move to handlers package later)
// func handleRoot(w http.ResponseWriter, r *http.Request) {
// 	fmt.Fprintf(w, "Hello from the backend!")
// }
