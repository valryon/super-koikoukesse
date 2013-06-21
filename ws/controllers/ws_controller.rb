require 'rubygems'
require 'sinatra'
require 'json'
require 'data_mapper'

class App < Sinatra::Base
  # Webservice URL

  # Get all games to exludes

  # Get all stats
  get '/stats.json' do
    Stats.all.to_json;
  end

  # Create a new stat line
  post '/stats.json' do
    puts "Creating new stat... hum"
  end
end
