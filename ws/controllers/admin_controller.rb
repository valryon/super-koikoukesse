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

  isFirst = true

  # Get the CSV file
  if params[:file] && (tmpfile = params[:file][:tempfile]) && (name = params[:file][:filename])

    # Delete everything
    Questions.destroy

    # Parse it!
    File.open(tmpfile, "r").each_line do |line|

      if isFirst
        isFirst = false
      else
        q = Questions.new

        cols = line.split(';')

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
