package handlers

import (
	"encoding/json"
	"github.com/andrewsoonqn/nuscuties-orbital/backend/internal/database"
	"github.com/avukadin/goapi/api"
	"github.com/gorilla/schema"
	log "github.com/sirupsen/logrus"
	"net/http"
)

type RegisterRequestParams struct {
	Username string
}
type RegisterResponse struct {
	Code int
}

func RegisterUser(w http.ResponseWriter, r *http.Request) {
	var params = RegisterRequestParams{}

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

	err = (*db).AddNewUser(params.Username)
	if err != nil {
		log.Error(err)
		api.InternalErrorHandler(w)
		return
	}

	var response = RegisterResponse{
		Code: http.StatusCreated,
	}

	w.Header().Set("Content-Type", "application/json")
	err = json.NewEncoder(w).Encode(response)
	if err != nil {
		log.Error(err)
		api.InternalErrorHandler(w)
		return
	}

	log.Info("Succesfully added new user: ", params.Username)
}
