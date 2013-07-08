require 'rubygems'
require 'sinatra'
require 'haml'
require 'data_mapper'

# Load up all models BEFORE the database init
Dir[File.dirname(__FILE__) + "/models/*.rb"].each do |file|
  require file
end

## Greetings
puts "=> Running in #{settings.environment} environment"

# Set up our general configs
set :root           , File.dirname(__FILE__)
set :public_folder  , File.dirname(__FILE__) + '/public'
set :app_file       , __FILE__
set :views          , File.dirname(__FILE__) + '/views'
set :haml           , :format => :html5
set :dump_errors    , true
set :logging        , true
set :raise_errors   , true

# -- All routes ending with .json return json content
before %r{.+\.json$} do
  content_type 'application/json'
end

# Data Mapper
puts "=> Initializing database..."

configure :development do
  DataMapper::Logger.new($stdout, :debug)
end
DataMapper.setup(:default, ENV['DATABASE_URL'] || "sqlite3://#{Dir.pwd}/db/dev.db")
DataMapper.auto_upgrade!

puts "=> Initializing database OK"

# Load up all controllers last
Dir[File.dirname(__FILE__) + "/controllers/*.rb"].each do |file|
  require file
end


