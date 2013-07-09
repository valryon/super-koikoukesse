require 'rubygems'
require 'sinatra'
require 'json'
require 'data_mapper'
require "base64"

# Webservice URL
#----------------------------------

# ******************************
# GAMES
# ******************************

# Get all games to exludes
get '/exclusions.json' do
  response = WsResponse.new

  #TODO

  return response.to_json
end

# ******************************
# PLAYERS
# ******************************

# Get player in database
get '/players.json/:playerId' do
  response = WsResponse.new

  if params[:playerId] != nil
    playerId = params[:playerId]

    # Find in database
    existingPlayer = Players.first(:playerId => playerId)

    if existingPlayer != nil
      response.code = ErrorCodes::OK
      response.data = existingPlayer.to_json
    else
      response.error = "Unknow player for id #{params[:playerId] }"
      response.code = ErrorCodes::UNKNOWOBJECT
    end

  else
    response.error = "Missing playerId argument in request."
    response.code = ErrorCodes::EMPTYREQUEST
  end

  return response.to_json
end

# Create player
post '/players.json' do
  response = WsResponse.new

  # Parse the json and check for all mandatory fields
  incomingjson = params[:r]
  doc = JSON.parse(incomingjson)

  playerId = doc["player"]
  platform = doc["platform"].to_i
  credits = doc["credits"].to_i
  coins = doc["coins"].to_i

  # Control if we have everything
  if playerId == nil or platform == nil or credits == nil or coins == nil
    response.code = ErrorCodes::INVALIDJSON
    response.error = "Invalid JSON"

  else

    # Exists in DB?
    existingPlayer = Players.first(:playerId => playerId)
    if existingPlayer != nil
      response.code = ErrorCodes::SERVERERROR
      response.error = "Player #{playerId} already exists!"
    else

      # Create the player
      p = Players.new

      savingIsOk = p.save
      if savingIsOk
        response.code = ErrorCodes::OK
      else
        response.code =  ErrorCodes::SERVERERROR
        response.error = "Errors during player save... "

        stat.errors.each do |e|
          response.error += e
        end #each

      end
    end
  end

  return response.to_json
end

# Add coins
post '/players/coins.json' do
  response = WsResponse.new

  #TODO

  return response.to_json
end

# Add credits
post '/players/credits.json' do
  response = WsResponse.new

  #TODO

  return response.to_json
end

# ******************************
# STATS
# ******************************

# Create a new stat line
post '/stats.json' do

  response = WsResponse.new

  incomingjson = params[:r]

  if incomingjson != nil
    doc = JSON.parse(incomingjson)
    stat = Stats.json_create(doc)
    savingIsOk = stat.save

    if savingIsOk
      response.code = ErrorCodes::OK
    else
      response.code =  ErrorCodes::SERVERERROR
      response.error = "Errors during stat save... "

      stat.errors.each do |e|
        response.error += e
      end
    end
  else
    response.code = ErrorCodes::INVALIDJSON
    response.error = "Invalid JSON"
  end

  return response.to_json
end
