class Stats
  include DataMapper::Resource

  property :id, Serial
  property :playerId, String
  property :score, Integer
  property :date, DateTime
  has n, :questionStats, 'QuestionStats'

  def initialize(playerId, score, date)
    @my_num = my_num
  end

  def to_json(*a)
    {
      'json_class'   => self.class.name,
      'data'         => [ first, last, exclude_end? ]
    }.to_json(*a)
  end

  def json_create(o)
    print 'Sans *'
    print o
    print 'Avec *'
    print *o
    # new(*o['data'])
  end
end

class QuestionStats
  include DataMapper::Resource

  property :id, Serial
  property :gameId, Integer
  property :result, Boolean

  belongs_to :stats, 'Stats'

end
