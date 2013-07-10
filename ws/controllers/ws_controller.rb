# Webservice
require 'rubygems'
require 'sinatra'
require 'json'
require 'data_mapper'
require "base64"

# ******************************
# GAMES
# ******************************

# Get all games to exludes
get '/ws/questions/ex.json' do
  response = WsResponse.new

  # Read all excluded questions
  excludedQuestions = Questions.all(:excluded => true, :fields => [:id])

  if excludedQuestions != nil

    response.code = ErrorCodes::OK

    # Build JSON
    rJson = "["
    excludedQuestions.each do |q|
      rJson += ("#{q.gameId},")
    end

    rJson += "]"

    response.data = rJson

  else
    response.code = ErrorCodes::SERVERERROR
    response.error = "Cannot retrieve questions information..."
  end

  return response.to_json
end

# ******************************
# PLAYERS
# ******************************

# Get player in database
#----------------------------------
get '/ws/players.json/:playerId' do
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
#----------------------------------
post '/ws/players.json' do

  # JSON example
  #{"player": "G1725278793", "credits": 2, "coins": 150, "platform": "ios"}

  response = WsResponse.new

  # Parse the json and check for all mandatory fields
  incomingjson = params[:r]
  doc = JSON.parse(incomingjson)

  p = Players.json_create(doc)

  if p != nil

    # Exists in DB?
    existingPlayer = Players.first(:playerId => p.playerId)

    if existingPlayer != nil
      response.code = ErrorCodes::SERVERERROR
      response.error = "Player #{p.playerId} already exists!"
    else
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
  else
    response.code =  ErrorCodes::INVALIDJSON
    response.error = "Cannot parse given JSON into player"
  end

  return response.to_json
end

# Add coins
#----------------------------------
post '/ws/players/coins.json' do
  response = WsResponse.new

  # JSON example
  # {"player": "G1725278793", "coins": 2}
  incomingjson = params[:r]
  doc = JSON.parse(incomingjson)

  playerId = doc["player"]
  coins = doc["coins"].to_i

  p = Players.first(:playerId => playerId)

  if p == nil
    response.error = "Unknow player for id #{playerId}"
    response.code = ErrorCodes::UNKNOWOBJECT
  else

    p.coins += coins

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

  return response.to_json
end

# Add credits
#----------------------------------
post '/ws/players/credits.json' do
  response = WsResponse.new

  # JSON example
  # {"player": "G1725278793", "credits": 2}
  incomingjson = params[:r]
  doc = JSON.parse(incomingjson)

  playerId = doc["player"]
  credits = doc["credits"].to_i

  p = Players.first(:playerId => playerId)

  if p == nil
    response.error = "Unknow player for id #{playerId}"
    response.code = ErrorCodes::UNKNOWOBJECT
  else

    p.credits += credits

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

  return response.to_json
end

# ******************************
# STATS
# ******************************

# Create a new stat line
#----------------------------------
post '/ws/stats.json' do

  response = WsResponse.new

  incomingjson = params[:r]

  if incomingjson != nil
    doc = JSON.parse(incomingjson)
    stat = Stats.json_create(doc)

    if stat != nil
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
      response.code =  ErrorCodes::SERVERERROR
      response.error = "Error on stat creation... "
    end
  else
    response.code = ErrorCodes::INVALIDJSON
    response.error = "Invalid JSON"
  end

  return response.to_json
end

# ******************************
# CONFIGURATION
# ******************************

# Get the configuration for a platform
#----------------------------------
get '/ws/config.json/:platform/:version' do
  response = WsResponse.new

  quizConfig = Configuration.first(:order => [ :version.desc ])

  if quizConfig == nil
    response.code =  ErrorCodes::SERVERERROR
    response.error = "Configuration not found"
  else
    response.code = ErrorCodes::OK
    response.data = quizConfig.to_json
  end

  return response.to_json
end
