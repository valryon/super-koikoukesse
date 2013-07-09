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

# Games
get '/admin/questions' do
  @questions = Questions.all
  haml :"admin/questions"
end

post '/admin/questions/upload' do

  # Get the CSV file
  if params[:file] && (tmpfile = params[:file][:tempfile]) && (name = params[:file][:filename])

    # Parse it!
    File.open(tmpfile, "r").each_line do |line|
      puts line
    end

  end

  # Redirect
  redirect '/admin/questions'
end
