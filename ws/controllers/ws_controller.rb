require 'rubygems'
require 'sinatra'
require 'json'
require 'data_mapper'
require "base64"

class App < Sinatra::Base
  # Webservice URL

  # Get all games to exludes

  # Get all stats
  get '/stats.json' do
    Stats.all.to_json;
  end

  # Create a new stat line
  post '/stats.json' do
    puts "Creating new stat..."
    incomingjson = params[:r]

    if incomingjson != nil
      doc = JSON.parse(incomingjson)
      stat = Stats.create_json(doc)
      "Ok"
    else
      "Ko missing 'r'"
    end
  end
end
