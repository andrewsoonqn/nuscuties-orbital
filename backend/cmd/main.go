package main

import (
	"fmt"
	"github.com/andrewsoonqn/nuscuties-orbital/backend/server/handlers"
	"github.com/go-chi/chi"
	log "github.com/sirupsen/logrus"
	"net/http"
)

func main() {
	log.SetReportCaller(true)
	fmt.Println("Starting backend...")

	var r = chi.NewRouter()
	handlers.Handler(r)

	err := http.ListenAndServe("localhost:8080", r)
	if err != nil {
		log.Error(err)
	}
}
