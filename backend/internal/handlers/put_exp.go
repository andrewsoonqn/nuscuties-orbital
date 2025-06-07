package handlers

import (
	"encoding/json"
	"github.com/andrewsoonqn/nuscuties-orbital/backend/internal/database"
	"github.com/avukadin/goapi/api"
	"github.com/go-chi/chi"
	"github.com/gorilla/schema"
	log "github.com/sirupsen/logrus"
	"net/http"
)

type PutRequestParams struct {
	NewExp int
}
type PutResponse struct {
	Code int
}

func PutExp(w http.ResponseWriter, r *http.Request) {
	var params = PutRequestParams{}

	username := chi.URLParam(r, "username")
	if username == "" {
		log.Error("Username is empty")
		api.InternalErrorHandler(w)
		return
	}

	var decoder *schema.Decoder = schema.NewDecoder()
	err := decoder.Decode(&params, r.URL.Query())
	if err != nil {
		log.Error(err)
		api.InternalErrorHandler(w)
		return
	}

	var db *database.Database
	db, err = database.NewDatabase()
	if err != nil {
		api.InternalErrorHandler(w)
		return
	}

	err = (*db).UpdateUserExp(username, params.NewExp)
	if err != nil {
		log.Error(err)
		api.InternalErrorHandler(w)
		return
	}

	var response = PutResponse{
		Code: http.StatusOK,
	}

	w.Header().Set("Content-Type", "application/json")
	err = json.NewEncoder(w).Encode(response)
	if err != nil {
		log.Error(err)
		api.InternalErrorHandler(w)
		return
	}

	log.Info("EXP updated successfully for user: ", username, ", now has ", params.NewExp, " EXP")
}
