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
    json =  "{"
    json += "\"version\": #{self.version},"
    json += "\"last_edit\": \"#{self.last_edit.strftime('%Y-%m-%d %H:%M:%S')}\","
    json += "\"config\": #{self.content}"
    json += "}"
  end

end
