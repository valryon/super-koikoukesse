#
# Stat for a complete game of the quizz
#
class Stats
  include DataMapper::Resource

  property :id, Serial
  property :playerId, String
  property :score, Integer
  property :mode, Integer
  property :difficulty, Integer
  property :date, DateTime
  has n, :questionStats, 'QuestionStats'

  def initialize(playerId, score, date, mode, difficulty, questionStats)
    self.playerId = playerId
    self.score = score
    self.date = date
    self.mode = mode
    self.difficulty = difficulty
    self.questionStats = questionStats
  end

  def self.json_create(o)

    # JSON example
    # {"player"=>"G1725278793", "score"=>455, "mode"=>0, "difficulty"=>0, "date"=>"2013-06-21 04:51:58",
    # "answers"=>[{"id"=>52, "result"=>false}, {"id"=>543, "result"=>true}, ...]}

    # Parse the json to get the right elements
    playerId = o["player"]
    score = o["score"].to_i
    mode = o["mode"].to_i
    difficulty = o["difficulty"].to_i
    date = DateTime.strptime(o["date"], '%Y-%m-%d %H:%M:%S')

    # Answers are in an array
    answers = nil

    o["answers"].each do |a|
      answers = Array.new

      questionId =  a["id"]
      questionResult = a["result"]

      answers.insert(QuestionStats.new(questionId, questionResult))
    end

    # Check if we filled everything
    if(playerId == nil or score == nil or mode == nil or difficulty == nil or date == nil or answers == nil)
      # TODO Nice error management
      return nil
    end

    # Create a new stat object
    return Stats.new(playerId, score, date, mode, difficulty, answers)
  end
end

#
# Stat for a single question of the quizz
#
class QuestionStats
  include DataMapper::Resource

  property :id, Serial
  property :questionId, Integer
  property :result, Boolean

  belongs_to :stats, 'Stats'

  def initialize(questionId, result)
    self.questionId = questionId
    self.result = result
  end

end
