#
# Stat for a complete game of the quizz
#
class Questions
  include DataMapper::Resource

  property :id, Serial
  property :gameId, Integer
  property :image, String
  property :titlePAL, String
  property :titleUS, String
  property :titleJAP, String
  property :platform, String
  property :genre, String
  property :publisher, String
  property :year, Integer
  property :excluded, Boolean


end
