require 'rubygems'
require 'sinatra'
require 'haml'

class App < Sinatra::Base
  # Root path
  get '/' do
    haml :"index"
  end
end
