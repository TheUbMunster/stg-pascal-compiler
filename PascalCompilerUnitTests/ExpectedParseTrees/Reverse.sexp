(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'R'
			Letter 'e'
			Letter 'v'
			Letter 'e'
			Letter 'r'
			Letter 's'
			Letter 'e'
			Letter 'S'
			Letter 't'
			Letter 'r'
			Letter 'i'
			Letter 'n'
			Letter 'g'
		)
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart)
			(ConstantDefinitionPart)
			(TypeDefinitionPart)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier
							Letter 's'
							Letter 't'
							Letter 'r'
						)
						Comma ','
						(Identifier
							Letter 'r'
							Letter 'e'
							Letter 'v'
							Letter 'e'
							Letter 'r'
							Letter 's'
							Letter 'e'
							Letter 'd'
						)
					)
					Colon ':'
					(TypeDenoter
						(TypeIdentifier
							(Identifier
								Letter 's'
								Letter 't'
								Letter 'r'
								Letter 'i'
								Letter 'n'
								Letter 'g'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier
							Letter 'i'
						)
					)
					Colon ':'
					(TypeDenoter
						(TypeIdentifier
							(Identifier
								Letter 'i'
								Letter 'n'
								Letter 't'
								Letter 'e'
								Letter 'g'
								Letter 'e'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter a string: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter a string: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter a string: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 's'
																				Letter 't'
																				Letter 'r'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 's'
																							Letter 't'
																							Letter 'r'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 's'
																								Letter 't'
																								Letter 'r'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 's'
																					Letter 't'
																					Letter 'r'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 's'
																					Letter 't'
																					Letter 'r'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 's'
																		Letter 't'
																		Letter 'r'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 's'
																			Letter 't'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 's'
															Letter 't'
															Letter 'r'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 's'
															Letter 't'
															Letter 'r'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ReadParameterList
											OpenParen '('
											(Ambiguous VariableAccess
												(VariableAccess
													(EntireVariable
														(VariableIdentifier
															(Identifier
																Letter 's'
																Letter 't'
																Letter 'r'
															)
														)
													)
												)
												(VariableAccess
													(ComponentVariable
														(FieldDesignator
															(FieldDesignatorIdentifier
																(Identifier
																	Letter 's'
																	Letter 't'
																	Letter 'r'
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous ReadlnParameterList
											(ReadlnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 's'
																		Letter 't'
																		Letter 'r'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 's'
																			Letter 't'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(ReadlnParameterList
												OpenParen '('
												(Ambiguous VariableAccess
													(VariableAccess
														(EntireVariable
															(VariableIdentifier
																(Identifier
																	Letter 's'
																	Letter 't'
																	Letter 'r'
																)
															)
														)
													)
													(VariableAccess
														(ComponentVariable
															(FieldDesignator
																(FieldDesignatorIdentifier
																	(Identifier
																		Letter 's'
																		Letter 't'
																		Letter 'r'
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 's'
																			Letter 't'
																			Letter 'r'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 's'
																						Letter 't'
																						Letter 'r'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 's'
																							Letter 't'
																							Letter 'r'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 's'
																				Letter 't'
																				Letter 'r'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 's'
																				Letter 't'
																				Letter 'r'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous WritelnParameterList
											(WritelnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 's'
																		Letter 't'
																		Letter 'r'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 's'
																			Letter 't'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(WritelnParameterList
												OpenParen '('
												(WriteParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 's'
																				Letter 't'
																				Letter 'r'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 's'
																							Letter 't'
																							Letter 'r'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 's'
																								Letter 't'
																								Letter 'r'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 's'
																					Letter 't'
																					Letter 'r'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 's'
																					Letter 't'
																					Letter 'r'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous AssignmentStatement
									(AssignmentStatement
										(Ambiguous VariableAccess
											(VariableAccess
												(EntireVariable
													(VariableIdentifier
														(Identifier
															Letter 'r'
															Letter 'e'
															Letter 'v'
															Letter 'e'
															Letter 'r'
															Letter 's'
															Letter 'e'
															Letter 'd'
														)
													)
												)
											)
											(VariableAccess
												(ComponentVariable
													(FieldDesignator
														(FieldDesignatorIdentifier
															(Identifier
																Letter 'r'
																Letter 'e'
																Letter 'v'
																Letter 'e'
																Letter 'r'
																Letter 's'
																Letter 'e'
																Letter 'd'
															)
														)
													)
												)
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															CharacterString ''''
														)
													)
												)
											)
										)
									)
									(AssignmentStatement
										(FunctionIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'v'
												Letter 'e'
												Letter 'r'
												Letter 's'
												Letter 'e'
												Letter 'd'
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															CharacterString ''''
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(ForStatement
										For 'for'
										(ControlVariable
											(EntireVariable
												(VariableIdentifier
													(Identifier
														Letter 'i'
													)
												)
											)
										)
										Walrus ':='
										(InitialValue
											(Expression
												(SimpleExpression
													(Term
														(Factor
															(FunctionDesignator
																(FunctionIdentifier
																	(Identifier
																		Letter 'l'
																		Letter 'e'
																		Letter 'n'
																		Letter 'g'
																		Letter 't'
																		Letter 'h'
																	)
																)
																(ActualParameterList
																	OpenParen '('
																	(Ambiguous ActualParameter
																		(ActualParameter
																			(Expression
																				(SimpleExpression
																					(Term
																						(Ambiguous Factor
																							(Factor
																								(BoundIdentifier
																									(Identifier
																										Letter 's'
																										Letter 't'
																										Letter 'r'
																									)
																								)
																							)
																							(Factor
																								(Ambiguous VariableAccess
																									(VariableAccess
																										(EntireVariable
																											(VariableIdentifier
																												(Identifier
																													Letter 's'
																													Letter 't'
																													Letter 'r'
																												)
																											)
																										)
																									)
																									(VariableAccess
																										(ComponentVariable
																											(FieldDesignator
																												(FieldDesignatorIdentifier
																													(Identifier
																														Letter 's'
																														Letter 't'
																														Letter 'r'
																													)
																												)
																											)
																										)
																									)
																								)
																							)
																							(Factor
																								(UnsignedConstant
																									(ConstantIdentifier
																										(Identifier
																											Letter 's'
																											Letter 't'
																											Letter 'r'
																										)
																									)
																								)
																							)
																							(Factor
																								(FunctionDesignator
																									(FunctionIdentifier
																										(Identifier
																											Letter 's'
																											Letter 't'
																											Letter 'r'
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(ActualParameter
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 's'
																								Letter 't'
																								Letter 'r'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 's'
																									Letter 't'
																									Letter 'r'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(ActualParameter
																			(ProcedureIdentifier
																				(Identifier
																					Letter 's'
																					Letter 't'
																					Letter 'r'
																				)
																			)
																		)
																		(ActualParameter
																			(FunctionIdentifier
																				(Identifier
																					Letter 's'
																					Letter 't'
																					Letter 'r'
																				)
																			)
																		)
																	)
																	CloseParen ')'
																)
															)
														)
													)
												)
											)
										)
										Downto 'downto'
										(FinalValue
											(Expression
												(SimpleExpression
													(Term
														(Factor
															(UnsignedConstant
																(UnsignedNumber
																	(UnsignedInteger
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(SimpleStatement
												(Ambiguous AssignmentStatement
													(AssignmentStatement
														(Ambiguous VariableAccess
															(VariableAccess
																(EntireVariable
																	(VariableIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 'v'
																			Letter 'e'
																			Letter 'r'
																			Letter 's'
																			Letter 'e'
																			Letter 'd'
																		)
																	)
																)
															)
															(VariableAccess
																(ComponentVariable
																	(FieldDesignator
																		(FieldDesignatorIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 'v'
																				Letter 'e'
																				Letter 'r'
																				Letter 's'
																				Letter 'e'
																				Letter 'd'
																			)
																		)
																	)
																)
															)
														)
														Walrus ':='
														(Expression
															(SimpleExpression
																(Term
																	(Ambiguous Factor
																		(Factor
																			(BoundIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 'v'
																					Letter 'e'
																					Letter 'r'
																					Letter 's'
																					Letter 'e'
																					Letter 'd'
																				)
																			)
																		)
																		(Factor
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 'r'
																								Letter 'e'
																								Letter 'v'
																								Letter 'e'
																								Letter 'r'
																								Letter 's'
																								Letter 'e'
																								Letter 'd'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 'r'
																									Letter 'e'
																									Letter 'v'
																									Letter 'e'
																									Letter 'r'
																									Letter 's'
																									Letter 'e'
																									Letter 'd'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(Factor
																			(UnsignedConstant
																				(ConstantIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 'v'
																						Letter 'e'
																						Letter 'r'
																						Letter 's'
																						Letter 'e'
																						Letter 'd'
																					)
																				)
																			)
																		)
																		(Factor
																			(FunctionDesignator
																				(FunctionIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 'v'
																						Letter 'e'
																						Letter 'r'
																						Letter 's'
																						Letter 'e'
																						Letter 'd'
																					)
																				)
																			)
																		)
																	)
																)
																(AddingOperator
																	Plus '+'
																)
																(Term
																	(Factor
																		(VariableAccess
																			(ComponentVariable
																				(IndexedVariable
																					(ArrayVariable
																						(Ambiguous VariableAccess
																							(VariableAccess
																								(EntireVariable
																									(VariableIdentifier
																										(Identifier
																											Letter 's'
																											Letter 't'
																											Letter 'r'
																										)
																									)
																								)
																							)
																							(VariableAccess
																								(ComponentVariable
																									(FieldDesignator
																										(FieldDesignatorIdentifier
																											(Identifier
																												Letter 's'
																												Letter 't'
																												Letter 'r'
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																					LeftSquareBracket '['
																					(IndexExpression
																						(Expression
																							(SimpleExpression
																								(Term
																									(Ambiguous Factor
																										(Factor
																											(BoundIdentifier
																												(Identifier
																													Letter 'i'
																												)
																											)
																										)
																										(Factor
																											(Ambiguous VariableAccess
																												(VariableAccess
																													(EntireVariable
																														(VariableIdentifier
																															(Identifier
																																Letter 'i'
																															)
																														)
																													)
																												)
																												(VariableAccess
																													(ComponentVariable
																														(FieldDesignator
																															(FieldDesignatorIdentifier
																																(Identifier
																																	Letter 'i'
																																)
																															)
																														)
																													)
																												)
																											)
																										)
																										(Factor
																											(UnsignedConstant
																												(ConstantIdentifier
																													(Identifier
																														Letter 'i'
																													)
																												)
																											)
																										)
																										(Factor
																											(FunctionDesignator
																												(FunctionIdentifier
																													(Identifier
																														Letter 'i'
																													)
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																					RightSquareBracket ']'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
													(AssignmentStatement
														(FunctionIdentifier
															(Identifier
																Letter 'r'
																Letter 'e'
																Letter 'v'
																Letter 'e'
																Letter 'r'
																Letter 's'
																Letter 'e'
																Letter 'd'
															)
														)
														Walrus ':='
														(Expression
															(SimpleExpression
																(Term
																	(Ambiguous Factor
																		(Factor
																			(BoundIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 'v'
																					Letter 'e'
																					Letter 'r'
																					Letter 's'
																					Letter 'e'
																					Letter 'd'
																				)
																			)
																		)
																		(Factor
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 'r'
																								Letter 'e'
																								Letter 'v'
																								Letter 'e'
																								Letter 'r'
																								Letter 's'
																								Letter 'e'
																								Letter 'd'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 'r'
																									Letter 'e'
																									Letter 'v'
																									Letter 'e'
																									Letter 'r'
																									Letter 's'
																									Letter 'e'
																									Letter 'd'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(Factor
																			(UnsignedConstant
																				(ConstantIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 'v'
																						Letter 'e'
																						Letter 'r'
																						Letter 's'
																						Letter 'e'
																						Letter 'd'
																					)
																				)
																			)
																		)
																		(Factor
																			(FunctionDesignator
																				(FunctionIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 'v'
																						Letter 'e'
																						Letter 'r'
																						Letter 's'
																						Letter 'e'
																						Letter 'd'
																					)
																				)
																			)
																		)
																	)
																)
																(AddingOperator
																	Plus '+'
																)
																(Term
																	(Factor
																		(VariableAccess
																			(ComponentVariable
																				(IndexedVariable
																					(ArrayVariable
																						(Ambiguous VariableAccess
																							(VariableAccess
																								(EntireVariable
																									(VariableIdentifier
																										(Identifier
																											Letter 's'
																											Letter 't'
																											Letter 'r'
																										)
																									)
																								)
																							)
																							(VariableAccess
																								(ComponentVariable
																									(FieldDesignator
																										(FieldDesignatorIdentifier
																											(Identifier
																												Letter 's'
																												Letter 't'
																												Letter 'r'
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																					LeftSquareBracket '['
																					(IndexExpression
																						(Expression
																							(SimpleExpression
																								(Term
																									(Ambiguous Factor
																										(Factor
																											(BoundIdentifier
																												(Identifier
																													Letter 'i'
																												)
																											)
																										)
																										(Factor
																											(Ambiguous VariableAccess
																												(VariableAccess
																													(EntireVariable
																														(VariableIdentifier
																															(Identifier
																																Letter 'i'
																															)
																														)
																													)
																												)
																												(VariableAccess
																													(ComponentVariable
																														(FieldDesignator
																															(FieldDesignatorIdentifier
																																(Identifier
																																	Letter 'i'
																																)
																															)
																														)
																													)
																												)
																											)
																										)
																										(Factor
																											(UnsignedConstant
																												(ConstantIdentifier
																													(Identifier
																														Letter 'i'
																													)
																												)
																											)
																										)
																										(Factor
																											(FunctionDesignator
																												(FunctionIdentifier
																													(Identifier
																														Letter 'i'
																													)
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																					RightSquareBracket ']'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Reversed string is: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 'v'
																				Letter 'e'
																				Letter 'r'
																				Letter 's'
																				Letter 'e'
																				Letter 'd'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'r'
																							Letter 'e'
																							Letter 'v'
																							Letter 'e'
																							Letter 'r'
																							Letter 's'
																							Letter 'e'
																							Letter 'd'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'r'
																								Letter 'e'
																								Letter 'v'
																								Letter 'e'
																								Letter 'r'
																								Letter 's'
																								Letter 'e'
																								Letter 'd'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 'v'
																					Letter 'e'
																					Letter 'r'
																					Letter 's'
																					Letter 'e'
																					Letter 'd'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 'v'
																					Letter 'e'
																					Letter 'r'
																					Letter 's'
																					Letter 'e'
																					Letter 'd'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'r'
																		Letter 'e'
																		Letter 'v'
																		Letter 'e'
																		Letter 'r'
																		Letter 's'
																		Letter 'e'
																		Letter 'd'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 'v'
																			Letter 'e'
																			Letter 'r'
																			Letter 's'
																			Letter 'e'
																			Letter 'd'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'r'
															Letter 'e'
															Letter 'v'
															Letter 'e'
															Letter 'r'
															Letter 's'
															Letter 'e'
															Letter 'd'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'r'
															Letter 'e'
															Letter 'v'
															Letter 'e'
															Letter 'r'
															Letter 's'
															Letter 'e'
															Letter 'd'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Reversed string is: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 'v'
																			Letter 'e'
																			Letter 'r'
																			Letter 's'
																			Letter 'e'
																			Letter 'd'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 'v'
																						Letter 'e'
																						Letter 'r'
																						Letter 's'
																						Letter 'e'
																						Letter 'd'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'r'
																							Letter 'e'
																							Letter 'v'
																							Letter 'e'
																							Letter 'r'
																							Letter 's'
																							Letter 'e'
																							Letter 'd'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 'v'
																				Letter 'e'
																				Letter 'r'
																				Letter 's'
																				Letter 'e'
																				Letter 'd'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 'v'
																				Letter 'e'
																				Letter 'r'
																				Letter 's'
																				Letter 'e'
																				Letter 'd'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Reversed string is: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 'v'
																			Letter 'e'
																			Letter 'r'
																			Letter 's'
																			Letter 'e'
																			Letter 'd'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 'v'
																						Letter 'e'
																						Letter 'r'
																						Letter 's'
																						Letter 'e'
																						Letter 'd'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'r'
																							Letter 'e'
																							Letter 'v'
																							Letter 'e'
																							Letter 'r'
																							Letter 's'
																							Letter 'e'
																							Letter 'd'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 'v'
																				Letter 'e'
																				Letter 'r'
																				Letter 's'
																				Letter 'e'
																				Letter 'd'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 'v'
																				Letter 'e'
																				Letter 'r'
																				Letter 's'
																				Letter 'e'
																				Letter 'd'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(EmptyStatement)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)