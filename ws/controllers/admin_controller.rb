require 'rubygems'
require 'sinatra'
require 'haml'
require 'data_mapper'

class App < Sinatra::Base
  # Application admin panel

  get '/admin' do
    haml :"admin/index"
  end

  # Stats list
  get '/admin/stats' do
    @stats = Stats.all(:order => [ :date.desc ])
    haml :"admin/stats"
  end
end
