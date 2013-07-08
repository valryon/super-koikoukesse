require 'rubygems'
require 'sinatra'
require 'haml'
require 'data_mapper'

# Application admin panel
#----------------------------------

get '/admin' do
  haml :"admin/index"
end

# players list
get '/admin/players' do
  @players = Players.all(:order => [ :creation_date.desc ])
  haml :"admin/players"
end

# Stats list
get '/admin/stats' do
  @stats = Stats.all(:order => [ :date.desc ])
  haml :"admin/stats"
end
