class Players
  include DataMapper::Resource

  property :id, Serial
  property :playerId, String
  property :creation_date, DateTime
  property :credits, Integer
  property :coins, Integer


  def to_json(*a)
    "Wabon player TODO"
  end

  def self.json_create(o)

  end
end
