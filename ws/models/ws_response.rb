#
# Standard response for the Webservice
#
class WsResponse

  attr_accessor :code
  attr_accessor :error
  attr_accessor :data

  def initialize
    code = ErrorCodes::OK
  end

  def to_json(*a)
    "{
      \"c\": #{@code}
      \"e\": \"#{@error}\"
      \"r\": \"#{@data}\"
    }"
  end

end
