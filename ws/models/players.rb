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

  #
  # Create player from JSON
  #
  def self.json_create(o)

    playerId = o["playerId"]
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
