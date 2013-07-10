require 'rubygems'
require 'sinatra'
require 'haml'

# Login
#----------------------------------
helpers do

  def login?
    if session[:username].nil?
      return false
    else
      return true
    end
  end

  def username
    return session[:username]
  end

  def auth
    if !login?
      redirect '/login'
    end
  end

end

get "/login" do
  haml :"login"
end

post "/login" do
  if params[:username].eql? 'admin' and params[:password].eql? 'Le koikou c\'est fantastique !'
    session[:username] = params[:username]
    redirect "/"
  else
    redirect "/login"
  end
end

get "/logout" do
  session[:username] = nil
  redirect "/"
end
