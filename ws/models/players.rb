#
# User of the game
#
class Players
  include DataMapper::Resource

  property :id, Serial
  property :playerId, String
  property :creation_date, DateTime
  property :credits, Integer
  property :coins, Integer
  property :platform, String

  def initialize(playerId, credits, coins, platform)
    self.creation_date = DateTime.now
    self.playerId = playerId
    self.credits = credits
    self.coins = coins
    self.platform = platform
  end

  def to_json(*a)
    # {"id": "G1728633519", "date": "2013-03-27T11:22:51.96Z", "credits": 2500, "coins": 3}

    "{
      \"id\": #{self.playerId},
      \"date\": #{self.creation_date},
      \"credits\": #{self.credits},
      \"coins\": #{self.coins},
      \"platform\": #{self.platform},
    }"
  end

  #
  # Create player from JSON
  #
  def self.json_create(o)

    playerId = o["player"]
    platform = o["platform"]
    credits = o["credits"].to_i
    coins = o["coins"].to_i

    if playerId != nil and platform != nil and credits != nil and coins != nil
      # Create the player
      p = Players.new(playerId, credits, coins, platform)
      return p
    end

    return nil

  end
end
