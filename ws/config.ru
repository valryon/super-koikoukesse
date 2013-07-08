# config.ru is used by shotgun (local dev server)
require './app.rb'
run Sinatra::Application
