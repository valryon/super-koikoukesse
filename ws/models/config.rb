#
# Configuration
#
class Configuration
  include DataMapper::Resource

  property :id, Serial
  property :version, Integer
  property :last_edit, DateTime
  property :content, String,   :length => 10024

  def initialize
    self.version = 1
    self.last_edit = DateTime.now
    self.content = File.open("./public/default.json", 'rb') { |file| file.read }
  end

  def to_json(*a)
    "{
      \"version\": #{self.version},
      \"last_edit\": #{self.last_edit},
      \"config\": \"#{self.content}\"
    }"
  end

end
