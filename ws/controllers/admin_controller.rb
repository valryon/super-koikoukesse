require 'rubygems'
require 'sinatra'
require 'haml'
require 'data_mapper'

class App < Sinatra::Base
  # Application admin panel

  # Stats list
  get '/admin/stats' do
    @stats = Stats.all;
    haml :"stats/index"
  end
end
