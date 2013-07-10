require 'rubygems'
require 'sinatra'
require 'haml'
require 'data_mapper'

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
enable :sessions

# Set our own config
$io_encryption = false

# -- All routes ending with .json return json content
before %r{.+\.json$} do
  content_type 'application/json'
end
# -- Same for .xml
before %r{.+\.xml$} do
  content_type 'application/xml'
end

# Load up all models BEFORE the database init
Dir[File.dirname(__FILE__) + "/models/*.rb"].each do |file|
  require file
end

# Data Mapper
puts "=> Initializing database..."

DataMapper.setup(:default, ENV['DATABASE_URL'] || "sqlite3://#{Dir.pwd}/db/dev.db")

configure :development do
  DataMapper::Logger.new($stdout, :debug)
end

DataMapper.auto_upgrade!
DataMapper.finalize

puts "=> Initializing database OK"

# Load up all controllers and stuff last
Dir[File.dirname(__FILE__) + "/tools/*.rb"].each do |file|
  require file
end
Dir[File.dirname(__FILE__) + "/controllers/*.rb"].each do |file|
  require file
end



