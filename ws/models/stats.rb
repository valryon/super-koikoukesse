class Stats
  include DataMapper::Resource

  property :id, Serial
  property :playerId, String
  property :score, Integer
  property :date, DateTime
end
