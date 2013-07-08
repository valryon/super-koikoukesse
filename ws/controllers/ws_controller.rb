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

    incomingjson = params[:r]

    if incomingjson != nil
      doc = JSON.parse(incomingjson)
      stat = Stats.json_create(doc)
      savingIsOk = stat.save

      if savingIsOk
        '{ "code": 0 }'
      else
        stat.errors.each do |e|
          puts e
        end
        '{ "code": 100 }'
      end
    else
      # TODO Nice error management
      '{ "code": 100 }'
    end
  end
end
