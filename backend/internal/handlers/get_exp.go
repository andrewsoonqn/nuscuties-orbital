package handlers

import (
	"encoding/json"
	"net/http"

	"github.com/andrewsoonqn/nuscuties-orbital/backend/internal/database"
	"github.com/avukadin/goapi/api"
	"github.com/go-chi/chi"
	log "github.com/sirupsen/logrus"
)

type ExpResponse struct {
	Exp  int
	Code int
}

func GetExp(w http.ResponseWriter, r *http.Request) {
	username := chi.URLParam(r, "username")
	if username == "" {
		log.Error("Username is empty")
		api.InternalErrorHandler(w)
		return
	}

	var db *database.Database
	db, err := database.NewDatabase()
	if err != nil {
		api.InternalErrorHandler(w)
		return
	}

	var expDetails *database.ExpDetails
	expDetails = (*db).GetUserExp(username)
	if expDetails == nil {
		log.Error("expDetails empty")
		api.InternalErrorHandler(w)
		return
	}

	var response = ExpResponse{
		Exp:  (*expDetails).Exp,
		Code: http.StatusOK,
	}

	w.Header().Set("Content-Type", "application/json")
	err = json.NewEncoder(w).Encode(response)
	if err != nil {
		log.Error(err)
		api.InternalErrorHandler(w)
		return
	}

	log.Info("EXP retrieved successfully for user: ", username)
}
