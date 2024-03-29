require 'rubygems'
require 'sinatra'
require 'haml'
require 'data_mapper'

# Application admin panel
#----------------------------------

# Admin home
get '/?' do
  # Redirect
  redirect '/admin'
end

get '/admin/?' do
  auth
  haml :"admin/index"
end

# Configuration
get '/admin/config/?' do
  auth

  @quizConfig = Configuration.first(:order => [ :version.desc ])

  if @quizConfig == nil
    puts "Creating new config..."
    @quizConfig = Configuration.new
    @quizConfig.save
  end

  haml :"admin/config"
end

post '/admin/config/update' do
  auth

  content = params[:content]
  version = params[:version].to_i

  if content != nil and version != nil
    puts "Updating config..."
    c = Configuration.new
    c.version = version + 1
    c.content = content
    c.save
  end

  # Redirect
  redirect '/admin/config'
end

# players list
get '/admin/players/?' do
  auth

  @players = Players.all(:order => [ :creation_date.desc ])
  haml :"admin/players"
end

# Stats list
get '/admin/stats/?' do
  auth

  @stats = Stats.all(:order => [ :date.desc ])
  haml :"admin/stats"
end

# Games
get '/admin/questions/?' do
  auth

  @questions = Question.all
  haml :"admin/questions"
end

get '/admin/questions/export.json' do
  auth

  games = Question.all
  return "{ \"games\": "+ games.to_json + "}"
end
get '/admin/questions/export.xml' do
  auth

  games = Question.all
  return "<?xml version=\"1.0\"?>"+ games.to_xml
end

get '/admin/questions/exclude/:id' do
  auth

  qId = params[:id].to_i

  if qId != nil

    # Find the matching question
    q = Question.first(:gameId => qId)

    if q != nil
      # Change exclusion
      q.excluded = !q.excluded

      # Save
      q.save
    end

  end

  # Redirect
  redirect '/admin/questions'
end

post '/admin/questions/upload' do
  auth
  isFirst = true

  # Get the CSV file
  if params[:file] && (tmpfile = params[:file][:tempfile]) && (name = params[:file][:filename])

    # Delete everything
    Question.destroy

    # Parse it!
    File.open(tmpfile, "r").each_line do |line|

      if isFirst
        isFirst = false
      else
        q = Question.new

        cols = line.split(';')

        q.gameId = cols[0].to_i
        q.image = cols[1]
        q.titlePAL = cols[2]
        q.titleUS = cols[3]
        q.platform = cols[4]
        q.genre = cols[5]
        q.publisher = cols[6]
        q.year = cols[7].to_i

        if cols[8].chomp.eql? "true"
          q.excluded = true
        else
          q.excluded = false
        end

        savingOk = q.save

        if savingOk == false
          puts "Saving error!"
          return
        end
      end
    end


  end

  # Redirect
  redirect '/admin/questions'
end
