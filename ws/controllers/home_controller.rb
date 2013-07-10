require 'rubygems'
require 'sinatra'
require 'haml'

# Root path
#----------------------------------

get '/' do
  haml :"index"
end
